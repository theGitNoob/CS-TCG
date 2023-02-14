using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
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

///<summary>
///Represents a condition for an `Effect`
///</summary>
public static class Condition
{
    ///<summary>
    ///Checks if the condition is correct
    ///</summary>
    ///<param name="conditionString">The condition as a string</param>
    ///<param name="imports">The imports for the condition used by `CSharpScript`</param>
    ///<exception cref="ArgumentNullException">Thrown when the condition is null</exception>
    ///<exception cref="CompilationErrorException">Thrown when the condition is not correct</exception>
    ///<returns>True if the condition is correct</returns>
    public static bool CheckIsCorrect<T>(string conditionString, params string[] imports)
    {
        if (conditionString == null) throw new ArgumentNullException(nameof(conditionString));

        var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly).AddImports(imports);

        var script = CSharpScript.Create<Func<T, T, bool>>(conditionString, options);

        var compileResult = script.Compile();

        if (compileResult.Length > 0)
            throw new CompilationErrorException(compileResult[0].GetMessage(), compileResult);

        return true;
    }
    ///<summary>
    ///Evaluates the condition
    ///</summary>
    ///<param name="conditionString">The condition as a string</param>
    ///<param name="p1">The first parameter</param>
    ///<param name="p2">The second parameter</param>
    ///<param name="imports">The imports for the condition used by `CSharpScript`</param>
    ///<exception cref="ArgumentNullException">Thrown when the condition is null</exception>
    ///<exception cref="CompilationErrorException">Thrown when the condition is not correct</exception>
    public static bool Evaluate<T>(string conditionString, T p1, T p2, params string[] imports)
    {
        if (p1 == null) throw new ArgumentNullException(nameof(p1));
        if (p2 == null) throw new ArgumentNullException(nameof(p2));

        var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly).AddImports(imports);

        var scriptRunner = CSharpScript.EvaluateAsync<Func<T, T, bool>>(conditionString, options);

        scriptRunner.Wait();

        bool result = scriptRunner.Result.Invoke(p1, p2);

        scriptRunner.Dispose();

        return result;

    }


}
///<summary>
///Represents an action for an `Effect`
///</summary>
public static class Action
{
    ///<summary>
    ///Executes the action
    ///</summary>
    ///<param name="actionString">The action as a string</param>
    ///<param name="p1">The first parameter</param>
    ///<param name="p2">The second parameter</param>
    ///<param name="imports">The imports for the action used by `CSharpScript`</param>
    ///<exception cref="ArgumentNullException">Thrown when the action is null</exception>
    ///<exception cref="CompilationErrorException">Thrown when the action is not correct</exception>
    public static void Execute<T>(string actionString, T p1, T p2, params string[] imports)
    {
        if (p1 == null) throw new ArgumentNullException(nameof(p1));
        if (p2 == null) throw new ArgumentNullException(nameof(p2));

        var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly).AddImports(imports);

        var scriptRunner = CSharpScript.EvaluateAsync<Action<T, T>>(actionString, options);

        scriptRunner.Wait();

        scriptRunner.Result.Invoke(p1, p2);

        scriptRunner.Dispose();
    }

    ///<summary>
    ///Checks if the action is correct
    ///</summary>
    ///<param name="actionString">The action as a string</param>
    ///<param name="import">The imports for the action used by `CSharpScript`</param>
    ///<exception cref="ArgumentNullException">Thrown when the action is null</exception>
    ///<exception cref="CompilationErrorException">Thrown when the action is not correct</exception>
    ///<returns>True if the action is correct</returns>
    public static bool CheckIsCorrect<T>(string actionString, params string[] import)
    {
        if (actionString == null) throw new ArgumentNullException(nameof(actionString));

        var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly).AddImports(import);

        var script = CSharpScript.Create<Action<T, T>>(actionString, options);

        var compileResult = script.Compile();

        if (compileResult.Length > 0)
            throw new CompilationErrorException(compileResult[0].GetMessage(), compileResult);

        return true;
    }
}
