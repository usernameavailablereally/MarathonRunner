using UnityEngine;

internal static class StringConstants
{
    public const string OBSTACLE_LAYER = "Obstacle";
    public const string GROUND_LAYER = "Ground";
    public const string JUMP_INPUT_NAME = "Jump";
    public const string RESTART_INPUT_NAME = "Restart";
    public const string GAME_OVER_INPUT_NAME = "GameOver";
    public static readonly int JumpTriggerName = Animator.StringToHash("Jump");
}