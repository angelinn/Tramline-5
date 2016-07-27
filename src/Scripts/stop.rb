class Stop
  attr_accessor :name
  attr_accessor :line
  attr_accessor :direction
  attr_accessor :day
  attr_accessor :timings

  def initialize(line)
    @line = line
  end
end
