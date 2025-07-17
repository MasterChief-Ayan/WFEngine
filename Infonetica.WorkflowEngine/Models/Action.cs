namespace Infonetica.WorkflowEngine.Models;

/// <summary>
/// Represents a workflow action that transitions from one state to another
/// Example: "Submit Application", "Approve Request"
/// </summary>
public record Action(
    /// <summary>
    /// Unique identifier for this action
    /// Example: "approve", "reject"
    /// </summary>
    string ActionId,
    
    /// <summary>
    /// Display name shown to users
    /// Example: "Approve Application"
    /// </summary>
    string ActionName,
    
    /// <summary>
    /// Description of what this action does
    /// </summary>
    string ActionDescription,
    
    /// <summary>
    /// List of states where this action can be executed
    /// Example: ["pending", "under_review"]
    /// </summary>
    List<string> OriginStates,
    
    /// <summary>
    /// Target state after action execution
    /// Example: "approved"
    /// </summary>
    string DestinationState,
    
    /// <summary>
    /// Whether this action is currently enabled
    /// </summary>
    bool IsEnabled
);