namespace StrategyGame.Decorators
{
    public static class DecoratorExtensions
    {
        public static bool HasDecorator<T>(this Unit unit) where T : UnitDecorator
        {
            if (unit is T) return true;
            if (unit is UnitDecorator decorator)
                return decorator.HasDecorator<T>();
            return false;
        }
    }
}
