<template>
  <nav>
    <v-app-bar flat app>
      <v-app-bar-title class="text-uppercase" >
        <span>DSP</span>
        <span class="font-weight-light">Tools</span>
      </v-app-bar-title>
    </v-app-bar>

    <v-navigation-drawer app permanent class="primary">
      <v-list>
        <v-list-item v-for="link in links" :key="link.route" router :to="link.route">
          <v-list-item-action>
            <v-icon class="white--text">{{link.icon}}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title class="white--text">{{link.text}}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

  </nav>
</template>

<script>
export default {
  name: "Navbar",
  data() {
    return {
      links: [
        { icon: 'home', text: 'Main Page', route:'/' },
        { icon: 'backup_table', text: 'Item Collection', route:'/items' },
      ]
    }
  }
}
</script>

<style scoped>

</style>
