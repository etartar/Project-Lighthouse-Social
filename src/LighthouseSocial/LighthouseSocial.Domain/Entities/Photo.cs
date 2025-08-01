﻿using LighthouseSocial.Domain.Common;
using LighthouseSocial.Domain.ValueObjects;

namespace LighthouseSocial.Domain.Entities;

public class Photo : EntityBase
{
    protected Photo() { }

    public Photo(Guid id, Guid userId, Guid lighthouseId, string filename, PhotoMetadata metadata)
    {
        Id = id != Guid.Empty ? id : Guid.NewGuid();
        UserId = userId;
        LighthouseId = lighthouseId;
        Filename = filename;
        UploadDate = DateTime.UtcNow;
        Metadata = metadata;
    }

    public Guid UserId { get; private set; }
    public Guid LighthouseId { get; private set; }
    public string Filename { get; private set; }
    public DateTime UploadDate { get; private set; }
    public PhotoMetadata Metadata { get; private set; }

    public List<Comment> Comments { get; } = [];
}
