require 'oj'

class JsonHelper
  def self.dump_to_file(name, data)
    File.open(name, 'w') { |file| file.write(Oj::dump data, :indent => 2) }
  end

  def self.load_from_file(name)
    json = File.read(name, :encoding => 'utf-8')
    Oj::load(json)
  end
end
