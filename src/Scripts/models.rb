class Arrival
  attr_accessor :name
  attr_accessor :line
  attr_accessor :direction
  attr_accessor :day
  attr_accessor :timings

  def initialize(line)
    @line = line
  end
end

class Line
  attr_accessor :name
  attr_accessor :directions
end

class Direction
  attr_accessor :name
  attr_accessor :days
end

class Day
  attr_accessor :type
  attr_accessor :stops
end

class Stop
  attr_accessor :name
  attr_accessor :timings
end
