namespace Infonetica.WorkflowEngine.Models;

/// <summary>
/// Represents a workflow state/stage
/// Example: "Draft", "Approved", "Rejected"
/// </summary>
public record State(
    /// <summary>
    /// Unique state identifier
    /// </summary>
    string StateId,
    
    /// <summary>
    /// Display name for users
    /// </summary>
    string StateName,
    
    /// <summary>
    /// State description
    /// </summary>
    string StateDescription,
    
    /// <summary>
    /// Is this the starting state?
    /// </summary>
    bool IsInitialState,
    
    /// <summary>
    /// Is this a final state?
    /// </summary>
    bool IsFinalState,
    
    /// <summary>
    /// Is this state currently enabled?
    /// </summary>
    bool IsEnabled
);