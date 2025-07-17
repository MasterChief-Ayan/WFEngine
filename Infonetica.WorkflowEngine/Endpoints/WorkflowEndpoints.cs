using Infonetica.WorkflowEngine.Models;
using Infonetica.WorkflowEngine.Services;
using Microsoft.AspNetCore.Mvc;

// HTTP endpoints for workflow operations

namespace Infonetica.WorkflowEngine.Endpoints;

/// <summary>
/// Contains all HTTP endpoints for workflow operations
/// </summary>
public static class WorkflowEndpoints
{
    /// <summary>
    /// Sets up all workflow API routes
    /// </summary>
    /// <param name="app">The web application instance</param>
    public static void MapWorkflowEndpoints(this WebApplication app)
    {
        // Create API group with "/api" prefix
        var group = app.MapGroup("/api");

        // ========== WORKFLOW DEFINITION ENDPOINTS ==========

        /// <summary>
        /// POST /api/definitions - Create new workflow definition
        /// </summary>
        group.MapPost("/definitions", ([FromBody] WorkflowDefinition definition, WorkflowService service) =>
        {
            var (newDefinition, error) = service.CreateDefinition(definition);
            
            // Return error or created definition
            return error != null ? Results.BadRequest(new { message = error }) : Results.Created($"/api/definitions/{newDefinition!.DefinitionId}", newDefinition);
        }).WithTags("Workflow Configuration");


        /// <summary>
        /// GET /api/definitions/{id} - Get workflow definition by ID
        /// </summary>
        group.MapGet("/definitions/{id}", (string id, InMemoryDataStore store) =>
        {
            return store.WorkflowDefinitions.TryGetValue(id, out var definition)
                ? Results.Ok(definition)
                : Results.NotFound( new { message = "Workflow Definition with given ID not found.", error = true});
        }).WithTags("Workflow Configuration");


        /// <summary>
        /// GET /api/definitions - Get all workflow definitions
        /// </summary>
        group.MapGet("/definitions", (InMemoryDataStore store) =>
        {
            return Results.Ok(store.WorkflowDefinitions.Values);
        }).WithTags("Workflow Configuration");

        // ========== WORKFLOW INSTANCE ENDPOINTS ==========

        /// <summary>
        /// POST /api/instances - Start new workflow instance
        /// </summary>
        group.MapPost("/instances", ([FromBody] StartInstanceRequest request, WorkflowService service) =>
        {
            var (newInstance, err) = service.StartInstance(request.DefinitionId);
            
            return err != null ? Results.NotFound(new { message = err, error = true }) : Results.Ok(newInstance);
        }).WithTags("Runtime");

        /// <summary>
        /// GET /api/instances/{id} - Get workflow instance by ID
        /// </summary>
        group.MapGet("/instances/{id}", (Guid id, InMemoryDataStore store) =>
        {
            return store.WorkflowInstances.TryGetValue(id, out var instance)
                ? Results.Ok(new { instance.InstanceId, instance.PresentState, instance.ChangeHistory })
                : Results.NotFound(new { message = "Workflow Instance with given ID not found.", error = true});
        }).WithTags("Runtime");

        /// <summary>
        /// GET /api/instances - Get all workflow instances
        /// </summary>
        group.MapGet("/instances", (InMemoryDataStore store) =>
        {
            return Results.Ok(store.WorkflowInstances.Values.Select(i => new { i.InstanceId, i.WorkflowDefinitionId, i.PresentState }));
        }).WithTags("Runtime");

        /// <summary>
        /// POST /api/instances/{id}/execute - Execute action on workflow instance
        /// </summary>
        group.MapPost("/instances/{id}/execute", (Guid id, [FromBody] ExecuteActionRequest request, WorkflowService service) =>
        {
            var (updatedInstance, error) = service.ExecuteAction(id, request.ActionId);
            
            return error != null ? Results.BadRequest(new { message = error }) : Results.Ok(updatedInstance);
        }).WithTags("Runtime");
    }

    // ========== REQUEST MODELS ==========

    /// <summary>
    /// Request to start new workflow instance
    /// </summary>
    public record StartInstanceRequest(string DefinitionId);
    
    /// <summary>
    /// Request to execute action on workflow instance
    /// </summary>
    public record ExecuteActionRequest(string ActionId);
}