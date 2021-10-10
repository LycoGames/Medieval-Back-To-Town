using System.Collections.Generic;

public interface IModifierProvider
{
    IEnumerable<float> GetAdditiveModifier(Stat stat);
    IEnumerable<float> GetPercentageModifiers(Stat stat);
}
