

using MongoDB.Bson.Serialization.Attributes;

namespace Task_Management_Service.Domain;
public class Project
{
    [BsonId]
    public string Reference { get; set; }
    public string Name { get; private set; }
    public string Description { get; set; }

    public Project(CreateProjectDto createProjectDto)
    {
        Reference = Guid.NewGuid().ToString();
        Name = createProjectDto.Name;
        Description = createProjectDto.Description;
    }
    public Project(UpdateProjectDto updateProjectDto)
    {
        Name = updateProjectDto.Name;
        Description = updateProjectDto.Description;
    }
    public Project()
    {
    }
}