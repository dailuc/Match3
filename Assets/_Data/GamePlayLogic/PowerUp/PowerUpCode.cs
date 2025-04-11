
public enum PowerUpCode 
{
    NoCode = 0,
    Bomb = 1,
    ColorBomb = 2,
    Lightning = 3,
}
public class PowerUpCodeParser
{
    public static PowerUpCode Fromstring(string PowerUpName)
    {
        return (PowerUpCode)System.Enum.Parse(typeof(PowerUpCode), PowerUpName);
    }
}
