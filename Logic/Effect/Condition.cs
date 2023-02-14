using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Effect;

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