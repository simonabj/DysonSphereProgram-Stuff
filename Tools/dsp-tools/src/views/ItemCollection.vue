<template>
  <v-app>
    <h1 class="subtitle-1 text--secondary">Item Collection</h1>

    <v-container fill-height>
      <v-row align="center" justify="center">
        <v-col cols="12" md="4" lg="3" v-for="item in items" :key="item.name">
          <v-card flat class="ma-3" align="center">
            <v-responsive class="pt-4">
              <v-avatar size="70">
                <img :src="item.icon" alt=""/>
              </v-avatar>
            </v-responsive>
            <v-card-title>
              {{ item.name }}
            </v-card-title>
            <v-card-subtitle align="left">
              <div class="long-description">{{ item.desc }}</div>
            </v-card-subtitle>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn text @click="item.expanded = !item.expanded">
                View Details
                <v-icon>{{ item.expanded ? 'mdi-chevron-up' : 'mdi-chevron-down' }}</v-icon>
              </v-btn>
            </v-card-actions>
            <v-expand-transition>
              <div v-show="item.expanded">
                <v-divider></v-divider>

                <v-card-text align="left">
                  <div><span class="font-weight-light">Type: </span>{{ item.type }}</div>
                  <div><span class="font-weight-light">Required Tech: </span>{{ item.tech }}</div>
                  <div v-show="item.isFluid"><span class="font-weight-bold orange--text">This is a fluid</span>
                  </div>
                </v-card-text>
              </div>
            </v-expand-transition>
          </v-card>
        </v-col>
      </v-row>
    </v-container>

  </v-app>
</template>

<script>
import json from "@/assets/dsp_recipe_dump.json"


export default {
  name: "ItemCollection",
  data() {
    return {
      show: false,
      items: Object.keys(json.item).map((key,) => {
        return {
          expanded: false,
          name: json.item[key].name,
          icon: json.item[key].iconPath + ".png",
          desc: json.item[key].description,
          tech: json.item[key].preTech ? json.item[key].preTech : "No tech required",
          type: json.item[key].type,
          isFluid: json.item[key].isFluid
        }
      })
    }
  },
  methods: {},
  computed: {
    // items:
    //   // {
    //   //   name: "Iron Ingot",
    //   //   icon: "/Icons/ItemRecipe/iron-plate.png",
    //   //   desc: "Basic raw material. Used to produce various iron components."
    //   // }
    // }
  }
}
</script>

<style scoped>

.long-description {
  overflow-y: auto;
  max-height: 80px;
}


</style>
