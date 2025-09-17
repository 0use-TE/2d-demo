using Godot;

namespace PlatformExplorer.PlayerScript
{
    public class PlayerInput
    {
        public float Horizontal => Input.GetAxis("move_left", "move_right");
        public float Vertical => Input.GetAxis("move_up", "move_down");

        public bool MeleeAttack => Input.IsActionJustPressed("melee_attack");
        public bool RemoteAttack => Input.IsActionJustPressed("remote_attack");

    }
}
