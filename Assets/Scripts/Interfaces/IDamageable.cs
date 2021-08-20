/* 
 * Units and Destructibles can be damaged
 */

public interface IDamageable 
{
    int GetHealth();

    void Damage(int amount, Unit by);  
    void Heal(int amount);
}
