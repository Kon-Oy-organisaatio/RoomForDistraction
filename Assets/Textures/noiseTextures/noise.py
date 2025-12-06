# noise_generator_selfcontained.py
import os
import math
import numpy as np
from PIL import Image

# Output settings
OUTPUT_DIR = "output"
os.makedirs(OUTPUT_DIR, exist_ok=True)

WIDTH = 512
HEIGHT = 512
SCALES = [2, 4, 8, 16, 32]  # 5 scales as requested

def save_gray(arr, filename):
    a = np.clip(arr * 255.0, 0, 255).astype(np.uint8)
    Image.fromarray(a, mode="L").save(os.path.join(OUTPUT_DIR, filename))

# ----------------------------
# Basic: White noise
# ----------------------------
def gen_white_noise(h=HEIGHT, w=WIDTH):
    return np.random.rand(h, w)

# ----------------------------
# Value noise (grid + bilerp)
# ----------------------------
def gen_value_noise(scale, h=HEIGHT, w=WIDTH):
    # grid size (ensure at least 2x2)
    gh = max(2, math.ceil(h / scale) + 1)
    gw = max(2, math.ceil(w / scale) + 1)
    grid = np.random.rand(gh, gw)

    ys = np.arange(h) / scale
    xs = np.arange(w) / scale
    yf = np.floor(ys).astype(int)
    xf = np.floor(xs).astype(int)
    y_frac = ys - yf
    x_frac = xs - xf

    # compute bilinear for each pixel using broadcasting
    v00 = grid[yf[:,None], xf[None,:]]
    v10 = grid[yf[:,None], xf[None,:]+1]
    v01 = grid[yf[:,None]+1, xf[None,:]]
    v11 = grid[yf[:,None]+1, xf[None,:]+1]

    i1 = v00 * (1 - x_frac[None,:]) + v10 * x_frac[None,:]
    i2 = v01 * (1 - x_frac[None,:]) + v11 * x_frac[None,:]
    result = i1 * (1 - y_frac[:,None]) + i2 * y_frac[:,None]
    return result

# ----------------------------
# Perlin noise (improved)
# ----------------------------
def _fade(t):
    return 6*t**5 - 15*t**4 + 10*t**3

def _grad(hash_vals, x, y):
    # convert hash to angle
    angles = (hash_vals % 8) * (math.pi * 2 / 8)
    gx = np.cos(angles)
    gy = np.sin(angles)
    return gx * x + gy * y

def gen_perlin_noise(scale, h=HEIGHT, w=WIDTH, tileable=False):
    # grid dims
    gx = math.ceil(w / scale) + 1
    gy = math.ceil(h / scale) + 1

    # permutation-like random ints for gradient selection
    rng = np.random.default_rng()
    grad_hash = rng.integers(0, 256, size=(gy, gx), dtype=np.int32)

    xs = np.arange(w) / scale
    ys = np.arange(h) / scale
    xf = np.floor(xs).astype(int)
    yf = np.floor(ys).astype(int)
    x_frac = xs - xf
    y_frac = ys - yf
    u = _fade(x_frac)
    v = _fade(y_frac)

    # corners' hash values
    h00 = grad_hash[yf[:,None], xf[None,:]        ]
    h10 = grad_hash[yf[:,None], xf[None,:] + 1    ]
    h01 = grad_hash[yf[:,None] + 1, xf[None,:]    ]
    h11 = grad_hash[yf[:,None] + 1, xf[None,:] + 1]

    # distance vectors
    dx00 = x_frac[None,:]
    dy00 = y_frac[:,None]
    dx10 = x_frac[None,:] - 1
    dy10 = y_frac[:,None]
    dx01 = x_frac[None,:]
    dy01 = y_frac[:,None] - 1
    dx11 = x_frac[None,:] - 1
    dy11 = y_frac[:,None] - 1

    n00 = _grad(h00, dx00, dy00)
    n10 = _grad(h10, dx10, dy10)
    n01 = _grad(h01, dx01, dy01)
    n11 = _grad(h11, dx11, dy11)

    lerp_x1 = n00 * (1 - u[None,:]) + n10 * u[None,:]
    lerp_x2 = n01 * (1 - u[None,:]) + n11 * u[None,:]
    res = lerp_x1 * (1 - v[:,None]) + lerp_x2 * v[:,None]

    # normalize to [0,1]
    res = (res - res.min()) / (res.max() - res.min() + 1e-12)
    return res

# ----------------------------
# FBM (fractal sum of Perlin)
# ----------------------------
def gen_fbm(scale, octaves=5, h=HEIGHT, w=WIDTH):
    total = np.zeros((h,w))
    amp = 1.0
    freq = 1.0
    max_amp = 0.0
    for _ in range(octaves):
        total += amp * gen_perlin_noise(scale / freq, h, w)
        max_amp += amp
        amp *= 0.5
        freq *= 2.0
    total /= max_amp
    return total

# ----------------------------
# Ridged multifractal (from perlin)
# ----------------------------
def gen_ridged(scale, octaves=5, h=HEIGHT, w=WIDTH):
    total = np.zeros((h,w))
    amp = 1.0
    freq = 1.0
    for _ in range(octaves):
        n = gen_perlin_noise(scale / freq, h, w)
        total += amp * (1.0 - np.abs(n))  # ridged
        amp *= 0.5
        freq *= 2.0
    total = (total - total.min()) / (total.max() - total.min() + 1e-12)
    return total

# ----------------------------
# Turbulence (sum abs Perlin)
# ----------------------------
def gen_turbulence(scale, octaves=5, h=HEIGHT, w=WIDTH):
    total = np.zeros((h,w))
    amp = 1.0
    freq = 1.0
    max_amp = 0.0
    for _ in range(octaves):
        n = gen_perlin_noise(scale / freq, h, w)
        total += amp * np.abs(n - 0.5) * 2.0  # center and abs
        max_amp += amp
        amp *= 0.5
        freq *= 2.0
    total /= max_amp
    total = (total - total.min()) / (total.max() - total.min() + 1e-12)
    return total

# ----------------------------
# Worley / Cellular noise (Euclidean distance)
# ----------------------------
def gen_worley(scale, feature_points_per_cell=1, h=HEIGHT, w=WIDTH):
    # create grid of cells approximately scale-sized
    cell_h = max(1, int(scale))
    cell_w = max(1, int(scale))
    cells_y = math.ceil(h / cell_h)
    cells_x = math.ceil(w / cell_w)

    # random feature points inside each cell
    rng = np.random.default_rng()
    features = rng.random((cells_y, cells_x, feature_points_per_cell, 2))  # offsets in cell coords

    ys = np.arange(h) / cell_h
    xs = np.arange(w) / cell_w
    yi = np.floor(ys).astype(int)
    xi = np.floor(xs).astype(int)
    yf = ys - yi
    xf = xs - xi

    # compute distance to nearest feature point (search neighbors)
    result = np.full((h,w), np.inf)
    for dy in (-1, 0, 1):
        for dx in (-1, 0, 1):
            ny = np.clip(yi[:,None] + dy, 0, cells_y-1)
            nx = np.clip(xi[None,:] + dx, 0, cells_x-1)

            # feature offsets for the selected neighbor cells
            # shape (h, w, k, 2)
            feats = features[ny[:,:,None], nx[None,:,:], :, :]  # careful broadcasting
            # pixel local coords [0..1) in cell
            local_x = xf[None,:]
            local_y = yf[:,None]
            # feature absolute positions relative to pixel
            fx = feats[...,0] + dx  # offset plus neighbor shift
            fy = feats[...,1] + dy
            dist = np.sqrt((fx - local_x[...,None])**2 + (fy - local_y[...,None])**2)
            min_dist = dist.min(axis=-1)
            result = np.minimum(result, min_dist)

    # normalize
    result = (result - result.min()) / (result.max() - result.min() + 1e-12)
    return result

# ----------------------------
# Generate everything
# ----------------------------
def main():
    print("Generating noise textures into", OUTPUT_DIR)
    types = [
        ("white", lambda s: gen_white_noise(HEIGHT, WIDTH)),
        ("value", gen_value_noise),
        ("perlin", gen_perlin_noise),
        ("fbm", gen_fbm),
        ("ridged", gen_ridged),
        ("turbulence", gen_turbulence),
        ("worley", gen_worley),
    ]

    for name, func in types:
        for scale in SCALES:
            print(f" - {name} scale {scale}")
            try:
                if name == "white":
                    arr = func(scale)
                else:
                    arr = func(scale)
            except Exception as e:
                print("   Error generating", name, ":", e)
                continue
            save_gray(arr, f"{name}_{scale}.png")

    print("Done.")

if __name__ == "__main__":
    main()
