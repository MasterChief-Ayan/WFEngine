namespace Infonetica.WorkflowEngine.Models;

/// <summary>
/// Represents a workflow definition template
/// Defines possible states, actions, and how they connect
/// </summary>
public record WorkflowDefinition(
    /// <summary>
    /// Unique identifier for this definition
    /// </summary>
    string DefinitionId,
    
    /// <summary>
    /// Display name for this definition
    /// </summary>
    string DefinitionName,
    
    /// <summary>
    /// Description of what this workflow is used for
    /// </summary>
    string DefinitionDescription,
    
    /// <summary>
    /// List of all possible states in this workflow
    /// </summary>
    List<State> WorkflowStates,
    
    /// <summary>
    /// List of all actions available in this workflow
    /// </summary>
    List<Action> WorkflowActions
);