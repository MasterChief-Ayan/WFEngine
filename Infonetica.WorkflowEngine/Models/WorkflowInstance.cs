namespace Infonetica.WorkflowEngine.Models;

/// <summary>
/// Represents a running workflow instance
/// Example: "John's Application", "Jane's Application" 
/// </summary>
public class WorkflowInstance
{
    /// <summary>
    /// Unique identifier for this instance
    /// </summary>
    public Guid InstanceId { get; }
    
    /// <summary>
    /// ID of the workflow definition this instance is based on
    /// </summary>
    public string WorkflowDefinitionId { get; }
    
    /// <summary>
    /// Current state of this instance
    /// </summary>
    public string PresentState { get; set; }
    
    /// <summary>
    /// Complete history of all changes made to this instance
    /// </summary>
    public List<Change> ChangeHistory { get; }

    /// <summary>
    /// Creates a new workflow instance
    /// </summary>
    /// <param name="definitionId">Workflow definition ID</param>
    /// <param name="initialState">Starting state</param>
    public WorkflowInstance(string definitionId, string initialState)
    {
        InstanceId = Guid.NewGuid();
        WorkflowDefinitionId = definitionId;
        PresentState = initialState;
        ChangeHistory = new List<Change>();
    }
}