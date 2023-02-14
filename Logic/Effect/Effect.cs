using System.Text.Json.Serialization;

namespace Effect;

///<summary>
///Represents an effect
///</summary>
public class Effect
{
    ///<summary>
    ///The condition of the effect as a string
    ///</summary>
    public string ConditionString { get; protected init; }

    ///<summary>
    ///The action for the effect as a string
    ///</summary>
    public string ActionString { get; protected init; }

    ///<summary>
    ///The imports for the condition and action used by `CSharpScript`
    ///</summary>
    public string[] Imports { get; protected set; }


    ///<summary>
    ///Creates a new Effect
    ///</summary>
    ///<param name="conditionString">The condition for the effect</param>
    ///<param name="actionString">The action for the effect</param>
    /// <param name="imports">Imports of ScriptRunner</param>
    ///<exception cref="ArgumentNullException">Thrown when the condition or action is null</exception>
    ///<returns>A new effect</returns>
    [JsonConstructor]
    public Effect(string conditionString, string actionString, params string[] imports)
    {
        ConditionString = conditionString ?? throw new ArgumentNullException(nameof(conditionString));
        ActionString = actionString ?? throw new ArgumentNullException(nameof(actionString));

        Imports = imports;
    }

    ///<summary>
    ///Checks if the condition and action are correct
    ///</summary>
    ///<param name="conditionString">The condition for the effect</param>
    ///<param name="actionString">The action for the effect</param>
    ///<param name="imports">The imports for the condition and action used by `CSharpScript`</param>
    ///<exception cref="ArgumentNullException">Thrown when the condition or action is null</exception>
    ///<returns>True if the condition and action are correct</returns> 
    public static bool CheckIsCorrect<T>(string conditionString, string actionString, params string[] imports)
    {
        if (conditionString == null) throw new ArgumentNullException(nameof(conditionString));
        if (actionString == null) throw new ArgumentNullException(nameof(actionString));

        return Condition.CheckIsCorrect<T>(conditionString, imports) && Action.CheckIsCorrect<T>(actionString, imports);
    }

    ///<summary>
    ///Checks if the effect can be activated
    ///</summary>
    ///<param name="p1">The first parameter</param>
    ///<param name="p2">The second parameter</param>
    ///<exception cref="ArgumentNullException">Thrown when the parameters are null</exception>
    ///<returns>True if the effect can be activated</returns>
    public bool CanActivate<T>(T p1, T p2)
    {
        if (p1 == null) throw new ArgumentNullException(nameof(p1));
        if (p2 == null) throw new ArgumentNullException(nameof(p2));

        return Condition.Evaluate<T>(this.ConditionString, p1, p2, this.Imports);
    }

    ///<summary>
    ///Activates the effect
    ///</summary>
    ///<param name="p1">The first parameter</param>
    ///<param name="p2">The second parameter</param>
    ///<exception cref="ArgumentNullException">Thrown when the parameters are null</exception>
    public void Activate<T>(T p1, T p2)
    {
        if (p1 == null) throw new ArgumentNullException(nameof(p1));
        if (p2 == null) throw new ArgumentNullException(nameof(p2));

        Action.Execute<T>(this.ActionString, p1, p2, this.Imports);
    }

}