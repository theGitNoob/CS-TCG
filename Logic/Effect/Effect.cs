using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Text.Json.Serialization;

namespace Effect
{
    ///<summary>
    ///Represents an effect
    ///</summary>
    public class Habilitie<T>
    {
        ///<summary>
        ///The condition for the effect
        ///</summary>
        [JsonIgnore]
        public Condition<T> _condition { get; private init; }

        ///<summary>
        ///The action for the effect
        ///</summary>
        [JsonIgnore]
        public Action<T> _action { get; private init; }

        ///<summary>
        ///The condition for the effect as a string
        ///</summary>
        public string ConditionString { get => _condition.ConditionString; }

        ///<summary>
        ///The action for the effect as a string
        ///</summary>
        public string ActionString { get => _action.ActionString; }

        ///<summary>
        ///Creates a new effect
        ///</summary>
        ///<param name="condition">The condition for the effect</param>
        ///<param name="action">The action for the effect</param>
        ///<exception cref="ArgumentNullException">Thrown when the condition or action is null</exception>
        ///<returns>A new effect</returns>
        public Habilitie(Condition<T> condition, Action<T> action)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (action == null) throw new ArgumentNullException(nameof(action));

            this._condition = condition;
            this._action = action;
        }

        ///<summary>
        ///Creates a new effect
        ///</summary>
        ///<param name="conditionString">The condition for the effect</param>
        ///<param name="actionString">The action for the effect</param>
        ///<example>
        ///new Habilitie("p1.Health > p2.Health", "p1.Health -= 10")
        ///</example>
        ///<exception cref="ArgumentNullException">Thrown when the condition or action is null</exception>
        ///<returns>A new effect</returns>
        [JsonConstructor]
        public Habilitie(string conditionString, string actionString)
        {
            if (conditionString == null) throw new ArgumentNullException(nameof(conditionString));
            if (actionString == null) throw new ArgumentNullException(nameof(actionString));

            this._condition = new Condition<T>(conditionString);
            this._action = new Action<T>(actionString);
        }

        ///<summary>
        ///Checks if the effect can be activated
        ///</summary>
        ///<param name="p1">The first player</param>
        ///<param name="p2">The second player</param>
        ///<exception cref="ArgumentNullException">Thrown when the first or second player is null</exception>
        ///<returns>True if the effect can be activated, false otherwise</returns>
        public bool CanActivate(T p1, T p2)
        {
            if (p1 == null || p2 == null) throw new ArgumentNullException();
            return _condition.Evaluate(p1, p2);
        }

        ///<summary>
        ///Activates the effect
        ///</summary>
        ///<param name="p1">The first player</param>
        ///<param name="p2">The second player</param>
        ///<exception cref="ArgumentNullException">Thrown when the first or second player is null</exception>
        public void Activate(T p1, T p2)
        {
            if (p1 == null || p2 == null) throw new ArgumentNullException();
            if (CanActivate(p1, p2))
                _action.Execute(p1, p2);
        }

    }

    ///<summary>
    ///Represents an contion for an `Effect`
    ///</summary>
    public class Condition<T>
    {
        ///<summary>
        ///The condition to evaluate
        ///</summary>
        [JsonIgnore]
        public Func<T, T, bool> _condition { get; init; }

        ///<summary>
        ///The condition as a string
        ///</summary>
        public string ConditionString { get; private init; }

        ///<summary>
        ///Evaluates the condition
        ///</summary>
        ///<param name="p1">The first player</param>
        ///<param name="p2">The second player</param>
        ///<exception cref="ArgumentNullException">Thrown when the first or second player is null</exception>
        ///<returns>True if the condition is true, false otherwise</returns>
        public bool Evaluate(T p1, T p2)
        {
            if (p1 == null || p2 == null) throw new ArgumentNullException();
            return _condition.Invoke(p1, p2);
        }

        ///<summary>
        ///Creates a new condition
        ///</summary>
        ///<param name="conditionString">The condition to evaluate</param>
        ///<example> 
        ///new Condition("p1.Health > p2.Health")
        ///</example>
        ///<exception cref="ArgumentNullException">Thrown when the condition is null</exception>
        ///<returns>A new condition</returns>
        [JsonConstructor]
        public Condition(string conditionString)
        {
            if (conditionString == null) throw new ArgumentNullException();

            var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly).AddImports("Player");

            var t = CSharpScript.EvaluateAsync<Func<T, T, bool>>(conditionString, options);

            t.Wait();

            this.ConditionString = conditionString;

            this._condition = t.Result;
        }

    }
    ///<summary>
    ///Represents an action for an `Effect`
    ///</summary>
    public class Action<T>
    {
        ///<summary>
        ///The action to execute
        ///</summary>
        [JsonIgnore]
        public Action<T, T> _action { get; init; }

        ///<summary>
        ///The action as a string
        ///</summary>
        public string ActionString { get; private init; }


        ///<summary>
        ///Executes the action
        ///</summary>
        ///<param name="p1">The first player</param>
        ///<param name="p2">The second player</param>
        ///<exception cref="ArgumentNullException">Thrown when the first or second player is null</exception>
        public void Execute(T p1, T p2)
        {
            if (p1 == null || p2 == null) throw new ArgumentNullException();
            _action.Invoke(p1, p2);
        }

        ///<summary>
        ///Creates a new action
        ///</summary>
        ///<param name="actionString">The action to execute</param>
        ///<example>
        ///new Action("p1.Health -= 10")
        ///</example>
        ///<exception cref="ArgumentNullException">Thrown when the action is null</exception>
        ///<returns>A new action</returns>
        [JsonConstructor]
        public Action(string actionString)
        {
            if (actionString == null) throw new ArgumentNullException();

            var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly).AddImports("Player");

            var t = CSharpScript.EvaluateAsync<Action<T, T>>(actionString, options);

            t.Wait();

            this.ActionString = actionString;

            this._action = t.Result;
        }
    }
}