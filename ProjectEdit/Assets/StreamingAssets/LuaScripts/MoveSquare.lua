require("ProjectEdit");

export = {
	moveSpeed = 3,
	movementSmoothness = 1,
	rotationSpeed = 5,
};

function start()
	local spriteRenderer = entity:GetComponent(SpriteRenderer);
	spriteRenderer.color = color.new(1, 0, 1, 1);
end

function update(ts)
	-- Rotation --
	-- transform.rotation = transform.rotation * quat.rotate(export.rotationSpeed * ts, -vec3.unit_z);
	local direction = vec3.new(0, 0, 0);

	if Input.GetKey("up") then
		direction.y = 1.0;
	end
	if Input.GetKey("down") then
		direction.y = -1.0;
	end
	if Input.GetKey("right") then
		direction.x = 1.0;
	end
	if Input.GetKey("left") then
		direction.x = -1.0;
	end
	
	transform.position = vec3.lerp(transform.position, transform.position + direction:normalize():scale(export.moveSpeed), ts * export.movementSmoothness);
end
