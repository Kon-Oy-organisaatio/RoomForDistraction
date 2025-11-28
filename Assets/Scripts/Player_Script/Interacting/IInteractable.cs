public interface IInteractable 
{
    void Interact();
    void ShowOutline();
    void HideOutline();
    string GetDescription();
    bool IsDisabled();
}