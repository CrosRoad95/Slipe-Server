-- Generated by CSharp.lua Compiler
local System = System
local SystemNumerics = System.Numerics
System.namespace("SlipeLua.Shared.Helpers", function (namespace)
  --/ <summary>
  --/ Adds some required numeric translations
  --/ </summary>
  namespace.class("NumericHelper", function (namespace)
    local ToRadians, ToDegrees, EulerToQuaternion, QuaternionToEuler, RotationBetweenPositions
    ToRadians = function (x)
      return x * 0.0174532924 --[[(float)(Math.PI / 180.0)]]
    end
    ToDegrees = function (x)
      return System.ToSingle(x * (57.295779513082323 --[[180.0 / Math.PI]]))
    end
    EulerToQuaternion = function (rotation)
      -- Default is XYZ
      -- Yaw = y-axis, Pitch = x-axis, Roll = z-axis
      return SystemNumerics.Quaternion.CreateFromYawPitchRoll(ToRadians(rotation.X), ToRadians(rotation.Y), ToRadians(rotation.Z))
    end
    QuaternionToEuler = function (q)
      local v1 = q.Z
      local v2 = q.X
      local v3 = q.Y
      local v4 = q.W


      local sinr_cosp = 2.0 * (v4 * v1 + v2 * v3)
      local cosr_cosp = 1.0 - 2.0 * (v1 * v1 + v2 * v2)
      local roll = math.Atan2(sinr_cosp, cosr_cosp)


      local sinp = 2.0 * (v4 * v2 - v3 * v1)
      local pitch
      if math.Abs(sinp) >= 1 then
        pitch = (math.Sign(sinp) > 0) and 3.1415926535897931 --[[Math.PI]] or - 3.1415926535897931 --[[Math.PI]]
      else
        pitch = math.Asin(sinp)
      end


      local siny_cosp = 2.0 * (v4 * v3 + v1 * v2)
      local cosy_cosp = 1.0 - 2.0 * (v2 * v2 + v3 * v3)
      local yaw = math.Atan2(siny_cosp, cosy_cosp)

      if yaw < 0 then
        yaw = yaw + (6.2831853071795862 --[[2 * Math.PI]])
      end

      if pitch < 0 then
        pitch = pitch + (6.2831853071795862 --[[2 * Math.PI]])
      end

      if roll < 0 then
        roll = roll + (6.2831853071795862 --[[2 * Math.PI]])
      end

      return SystemNumerics.Vector3(ToDegrees(System.ToSingle(yaw)), ToDegrees(System.ToSingle(pitch)), ToDegrees(System.ToSingle(roll)))
    end
    RotationBetweenPositions = function (position1, position2)
      local xAngle = ToDegrees(System.ToSingle(math.Asin((position1.Z - position2.Z) / SystemNumerics.Vector3.Distance(position2, position1))))
      local zAngle = - ToDegrees(System.ToSingle(math.Atan2(position1.X - position2.X, position1.Y - position2.Y)))
      return SystemNumerics.Vector3((xAngle < 0) and (xAngle + 360) or xAngle, 0, (zAngle < 0) and (zAngle + 360) or zAngle)
    end
    return {
      ToRadians = ToRadians,
      ToDegrees = ToDegrees,
      EulerToQuaternion = EulerToQuaternion,
      QuaternionToEuler = QuaternionToEuler,
      RotationBetweenPositions = RotationBetweenPositions,
      __metadata__ = function (out)
        return {
          methods = {
            { "EulerToQuaternion", 0x18E, EulerToQuaternion, System.Numerics.Vector3, System.Numerics.Quaternion },
            { "QuaternionToEuler", 0x18E, QuaternionToEuler, System.Numerics.Quaternion, System.Numerics.Vector3 },
            { "RotationBetweenPositions", 0x28E, RotationBetweenPositions, System.Numerics.Vector3, System.Numerics.Vector3, System.Numerics.Vector3 },
            { "ToDegrees", 0x18E, ToDegrees, System.Single, System.Single },
            { "ToRadians", 0x18E, ToRadians, System.Single, System.Single }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)