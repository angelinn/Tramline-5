require_relative 'models'

class Transformer
  def self.to_logical_hash(data)
    lines = data.flatten.group_by { |s| s.line }
    lines.each do |line, line_data|
      lines[line] = line_data.group_by { |s| s.direction }
      lines[line].each do |direction, direction_data|
        lines[line][direction] = direction_data.group_by { |g| g.day }
      end
    end
  end

  def self.to_csharp_ready_hash(hash)
    hash.map do |line_key, directions|
      line = Line.new
      line.name = line_key
      line.directions = directions.map do |dir_key, days|
        dir = Direction.new
        dir.name = dir_key
        dir.days = days.map do |day_key, stops|
          day = Day.new
          day.type = day_key
          day.stops = stops.map do |arrival|
            stop = Stop.new
            stop.name = arrival.name
            stop.timings = arrival.timings
            stop
          end.uniq { |stop| [stop.name, stop.timings] }
          day
        end
        dir
      end
      line
    end
  end
end
