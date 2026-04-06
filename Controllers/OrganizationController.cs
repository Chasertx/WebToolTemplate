using Microsoft.AspNetCore.Mvc;
using Template.Api.Models.Foundation.Organization;
using Template.Api.Models.Foundation.Organization.Exceptions;
using Template.Api.Services.Foundations.Organizations;

[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    //Variable for holding the organizationService
    private readonly IOrganizationService organizationService;

    /// <summary>
    /// Organization controller constructor injects
    /// and sets the organizationService class.
    /// </summary>
    /// <param name="orgService"></param>
    public OrganizationsController(IOrganizationService orgService) =>
        this.organizationService = orgService;

    /// <summary>
    /// POST api/organizations inserts a new organization
    /// record into the database using the broker.
    /// </summary>
    /// <param name="organization"></param>
    /// <returns></returns>
    [HttpPost]
    public async ValueTask<ActionResult<Organization>> PostOrganizationAsync([FromBody] Organization organization)
    {
        try
        {
            Organization addedOrganization =
                await this.organizationService.AddOrganizationAsync(organization);

            return CreatedAtAction(nameof(GetOrganizationById), new { id = addedOrganization.Id }, addedOrganization);
        }
        catch (OrganizationValidationException organizationValidationException)
        {
            return Conflict(organizationValidationException.Message);
        }
    }

    /// <summary>
    /// Gets a list of all organizations in the organization
    /// table.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<IQueryable<Organization>> GetAllOrganizations()
    {
        IQueryable<Organization> organizations =
            this.organizationService.RetrieveAllOrganizations();

        return Ok(organizations);
    }

    /// <summary>
    /// Temporary place holder for retrieving an organization
    /// by it's unique id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public ActionResult<Organization> GetOrganizationById(Guid id)
    {
        return Ok();
    }
}