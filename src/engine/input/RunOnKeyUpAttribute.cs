using Godot;

/// <summary>
///   Attribute for a method, that gets called when the defined key is released.
///   Can be applied multiple times.
/// </summary>
public class RunOnKeyUpAttribute : RunOnKeyAttribute
{
    public RunOnKeyUpAttribute(string godotInputName) : base(godotInputName)
    {
    }

    public override bool OnInput(InputEvent @event)
    {
        var before = HeldDown;
        if (!base.OnInput(@event) && before)
        {
            return CallMethod();
        }

        return false;
    }

    public override void OnProcess(float delta)
    {
    }
}
