using Infonetica.WorkflowEngine.Models;

namespace Infonetica.WorkflowEngine.Services;

/// <summary>
/// Main service for workflow business logic
/// Handles creating definitions, starting instances, and executing actions
/// </summary>
public class WorkflowService
{
    /// <summary>
    /// Data store for workflow data
    /// </summary>
    private readonly InMemoryDataStore _dataStore;

    /// <summary>
    /// Constructor with dependency injection
    /// </summary>
    /// <param name="dataStore">Data store instance</param>
    public WorkflowService(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    /// <summary>
    /// Creates and validates a new workflow definition
    /// </summary>
    /// <param name="definition">Workflow definition to create</param>
    /// <returns>Created definition or error message</returns>
    public (WorkflowDefinition? definition, string? error) CreateDefinition(WorkflowDefinition definition)
    {
        // Validate definition ID
        if (string.IsNullOrWhiteSpace(definition.DefinitionId))
        {
            return (null, "Workflow definition ID cannot be null or empty.");
        }
        
        // Check for duplicate ID
        if (_dataStore.WorkflowDefinitions.ContainsKey(definition.DefinitionId))
        {
            return (null, $"Workflow definition with ID '{definition.DefinitionId}' already exists.");
        }

        // Validate exactly one initial state
        if (definition.WorkflowStates.Count(s => s.IsInitialState) != 1)
        {
            return (null, "A workflow definition must have exactly one initial state.");
        }

        // Validate unique state IDs
        var stateIds = new HashSet<string>(definition.WorkflowStates.Select(s => s.StateId));
        if (stateIds.Count != definition.WorkflowStates.Count)
        {
            return (null, "State IDs within a definition must be unique.");
        }

        // Store the definition
        _dataStore.WorkflowDefinitions[definition.DefinitionId] = definition;
        return (definition, null);
    }

    /// <summary>
    /// Starts a new workflow instance
    /// </summary>
    /// <param name="definitionId">Workflow definition ID</param>
    /// <returns>Created instance or error message</returns>
    public (WorkflowInstance? instance, string? error) StartInstance(string definitionId)
    {
        // Find the workflow definition
        if (!_dataStore.WorkflowDefinitions.TryGetValue(definitionId, out var definition))
        {
            return (null, $"Workflow definition with ID '{definitionId}' not found.");
        }

        // Find the initial state
        var initialState = definition.WorkflowStates.SingleOrDefault(s => s.IsInitialState);
        if (initialState == null)
        {
            return (null, "Cannot start instance: Definition is invalid and lacks an initial state.");
        }

        // Create and store the instance
        var instance = new WorkflowInstance(definitionId, initialState.StateId);
        _dataStore.WorkflowInstances[instance.InstanceId] = instance;

        return (instance, null);
    }

    /// <summary>
    /// Executes an action on a workflow instance
    /// </summary>
    /// <param name="instanceId">Instance ID</param>
    /// <param name="actionId">Action ID</param>
    /// <returns>Updated instance or error message</returns>
    public (WorkflowInstance? instance, string? error) ExecuteAction(Guid instanceId, string actionId)
    {   
        // Basic validation
        if (Guid.Empty == instanceId)
        {
            return (null, "Instance ID cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(actionId))
        {
            return (null, "Action ID cannot be null or empty.");
        }

        // Find the workflow instance
        if (!_dataStore.WorkflowInstances.TryGetValue(instanceId, out var instance))
        {
            return (null, "Workflow instance not found.");
        }

        // Find the workflow definition
        if (!_dataStore.WorkflowDefinitions.TryGetValue(instance.WorkflowDefinitionId, out var definition))
        {
            return (null, "Underlying workflow definition for this instance not found.");
        }

        // Check if workflow is in final state
        var currentState = definition.WorkflowStates.FirstOrDefault(s => s.StateId == instance.PresentState);
        if (currentState != null && currentState.IsFinalState)
        {
            return (null, "Action cannot be executed as the workflow instance is in a final state.");
        }

        // Find the action
        var action = definition.WorkflowActions.FirstOrDefault(a => a.ActionId == actionId);
        if (action == null)
        {
            return (null, "Action not found in workflow definition.");
        }

        // Validate the action
        if (!action.OriginStates.Contains(instance.PresentState))
        {
            return (null, "Action is not valid for the current state of the workflow instance.");
        }

        if (!action.IsEnabled)
        {
            return (null, "This action is currently disabled.");
        }

        // Execute the action
        var change = new Change(action.ActionName, instance.PresentState, action.DestinationState, DateTime.UtcNow);
        instance.ChangeHistory.Add(change);
        instance.PresentState = action.DestinationState;

        return (instance, null);
    }
}