using System.Collections.Concurrent;
using Infonetica.WorkflowEngine.Models;

namespace Infonetica.WorkflowEngine.Services;

/// <summary>
/// In-memory data store for workflow definitions and instances
/// Note: Data is lost when application stops
/// </summary>
public class InMemoryDataStore
{
    /// <summary>
    /// Stores all workflow definitions
    /// </summary>
    public ConcurrentDictionary<string, WorkflowDefinition> WorkflowDefinitions { get; } = new();
    
    /// <summary>
    /// Stores all workflow instances
    /// </summary>
    public ConcurrentDictionary<Guid, WorkflowInstance> WorkflowInstances { get; } = new();
}