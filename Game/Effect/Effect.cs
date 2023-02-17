using System.Text.Json.Serialization;
using Compiler;
using Compiler.Syntax;
using Player;

namespace Effect;

///<summary>
///Represents an effect
///</summary>
public class Effect
{
    private SyntaxTree _syntaxTree { get; init; }
    
    private Compilation _compilation { get; init; }
    
    private Dictionary<VariableSymbol, object> _variables { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="effectString"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public Effect(string effectString)
    {
        if (string.IsNullOrEmpty(effectString))
            throw new ArgumentNullException($"{nameof(effectString)} should'nt be null or empty");

        _variables = new Dictionary<VariableSymbol, object>();
        EffectString = effectString;

        _syntaxTree = SyntaxTree.Parse(effectString);
        _compilation = new Compilation(_syntaxTree);

        EvaluationResult result = _compilation.Evaluate(_variables);
        var diagnostics = result.Diagnostics;

        foreach (var diagnostic in diagnostics)
        {
            throw new Exception(diagnostic.Message);
        }
    }

    public void Activate(SimplePlayer player)
    {
        EvaluationResult result = _compilation.Evaluate(_variables, player);
        var diagnostics = result.Diagnostics;

        foreach (var diagnostic in diagnostics)
        {
            throw new Exception(diagnostic.Message);
        }
    }

    /// <summary>
    /// Holds the Effect as a string to be compiled an executed later
    /// </summary>
    public string EffectString { get; set; }
}