Entity = {};
EntityMt = {};

local function new(entityHandle)
    return setmetatable({ entityHandle = entityHandle }, EntityMt);
end

function Entity.new(entityHandle)
    return new(entityHandle);
end

function Entity.AddComponent(entity, component)
    return InternalCalls.AddComponent(entity.entityHandle, component);
end

function Entity.GetComponent(entity, component)
    if entity:HasComponent(component) then
        return component.new(entity);
    end

    return nil;
end

function Entity.HasComponent(entity, component)
    return InternalCalls.HasComponent(entity.entityHandle, component.ToString());
end

EntityMt.__index = Entity;

function Entity.__eq(entity1, entity2)
    return entity1.entityHandle == entity2.entityHandle;
end

return setmetatable({}, EntityMt);
