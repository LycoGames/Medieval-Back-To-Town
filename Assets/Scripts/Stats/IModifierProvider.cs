using System.Collections.Generic;

public interface IModifierProvider
{
    IEnumerable<float> GetAdditiveModifiers(Stat stat);
    IEnumerable<float> GetPercentageModifiers(Stat stat);
}
