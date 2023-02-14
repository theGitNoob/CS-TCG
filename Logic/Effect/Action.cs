using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Effect;

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