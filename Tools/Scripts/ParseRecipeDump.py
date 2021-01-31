import re
import json

raw_file = open("gistfile1.txt", 'r')
raw_data = "".join(raw_file.readlines())
raw_file.close()

obj = {'items': {}, 'recipes': {}}

# Item match pattern. Groupes: ('item-id', 'Item Name')
item_name_ptrn = r'\[item-(.+?)\]\nname = """(.+?)"""(?:.|\n)+?iconPath = """(.+?)"""'

# Recipe match pattern. Groups: ('recipe-id','timespend(int)', 'method',
# 'iconPath', 'string-block of requirements', 'string-block of results)
recipe_ptrn = r'\[recipe-(.+?)\]\n(?:\n|.)+?timespend\s=\s(\d+?)\n(?:\n|.)*?type\s=\s"""(.+?)"""(' \
              r'?:.|\n)+?iconPath\s?=\s?"""(.*?)"""(?:.|\n)*?\[recipe-\1.items\]\n((?:.|\n)+?)\[' \
              r'recipe-\1\.results\]\n((?:[^\[]|\n)+)'

item_split_ptrn = r'"""item-(.+?)"""\s=\s(\d+?)\n'

obj['items'] = {
    match[0]: {'name': match[1], 'icon': match[2]} for match in re.findall(item_name_ptrn, raw_data)
}

obj['recipes'] = {
    match[0]: {
        'timespend': int(match[1]),
        'method': match[2],
        'icon': match[3] if match[3] else obj['items'][match[0]]['icon'],
        'requires': {
            item_id: int(amount) for (item_id, amount) in re.findall(item_split_ptrn, match[4])
        },
        'creates': {
            item_id: int(amount) for (item_id, amount) in re.findall(item_split_ptrn, match[5])
        }
    } for match in re.findall(recipe_ptrn, raw_data)
}

encoder = json.JSONEncoder(indent=3)
outfile = open("SensibleRecipeData.json", 'w')
outfile.writelines(encoder.iterencode(obj))
outfile.flush()
outfile.close()
