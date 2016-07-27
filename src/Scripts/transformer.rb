require_relative 'models'

class Transformer
  def self.to_logical_hash(data)
    lines = data.flatten.group_by { |s| s.line }
    lines.each do |key, value|
      lines[key] = value.group_by { |s| s.direction }
      lines[key].each { |k, v| lines[key][k] = v.group_by { |g| g.day } }
    end
  end

  def self.to_csharp_ready_hash(hash)
    hash.map do |key, value|
      line = Line.new
      line.name = key
      line.directions = value.map do |k, v|
        dir = Direction.new
        dir.name = k
        dir.days = v.map do |lelk, lelv|
          day = Day.new
          day.type = lelk
          day.stops = lelv.map do |arrival|
            stop = Stop.new
            stop.name = arrival.name
            stop.timings = arrival.timings
            stop
          end
          day
        end
        dir
      end
      line
    end
  end
end
