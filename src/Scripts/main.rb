require_relative 'transformer'
require_relative 'sumc_crawler'

all_arrivals = SumcScheduleFetcher.get_all_timings
stop_codes = SumcScheduleFetcher.stop_codes

puts 'Transforming data..'

logical_hash = Transformer.to_logical_hash(all_arrivals)
csharp_ready = Transformer.to_csharp_ready_hash(logical_hash, stop_codes)

puts 'Done..'

timestamp = Time.new.strftime("%Y%m%d%H%M")
JsonHelper.dump_to_file('grouped_' + timestamp + '.json', csharp_ready)
