namespace Common
{
    public enum Directions
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public enum GameStatesType
    {
        PLAYERSTATE,
        ENEMYSTATE,
        GAMEOVERSTATE,
        LEVELFINISHEDSTATE,
        LOBBYSTATE,
        LOADLEVELSTATE,
        GAMERETRYSTATE,
        GAMECOMPLETED
    }
    public enum StarTypes{
        PLAYERMOVES,
        NOKILL,
        ALLKILL,
        PICKBRIEFCASE,
        KILLDOGS,
        COMPLETED,
        NOKILLDOGS
    }
    public enum EnemyType
    {
        STATIC,
        PATROLLING,
        ROTATING_KNIFE,
        SNIPER,
        BIDIRECTIONAL,        
        DOGS,
        CIRCULAR_COP,
        GUARD_TORCH,
        TARGET,
        None
    }
    public enum KeyTypes{
        NONE,
        RED,
        BLUE,
        YELLOW
    }
     public enum NodeProperty
    {
        NONE,
        SPAWNPLAYER,
        TARGETNODE,
        TELEPORT

    }
    public enum InteractablePickup
    {
        NONE,
        BREIFCASE,
        STONE,
        BONE,
        SNIPER_GUN,
        DUAL_GUN,
        TRAP_DOOR,
        COLOR_KEY,
        AMBUSH_PLANT,
        GUARD_DISGUISE
    }
    public enum PlayerStates
    {
        IDLE,
        AMBUSH,
        DISGUISE,
        UNLOCK_DOOR,
        SHOOTING,
        THROWING,
        WAIT_FOR_INPUT,
        END_TURN,
        INTERMEDIATE_MOVE,
        DEAD,
        NONE

    }
    public enum EnemyStates
    {
        IDLE,
        MOVING,
        CHASE,
        RETURN_TO_PATH,
        CONSTANT_CHASE,
        DEATH
    }
    public enum KillMode
    {
        WALK,
        SHOOT,
        BOMB

    }
}