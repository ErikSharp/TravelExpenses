<template>
  <div>
    <v-toolbar fixed dark color="primary">
      <v-toolbar-title class="white--text">{{title}}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-menu bottom left>
        <v-btn dark slot="activator" icon>
          <v-icon>more_vert</v-icon>
        </v-btn>

        <v-list>
          <v-list-tile v-for="(item, i) in items" :key="i" @click="menuItemClicked(i)">
            <v-list-tile-title>{{ item.title }}</v-list-tile-title>
          </v-list-tile>
        </v-list>
      </v-menu>
    </v-toolbar>
  </div>
</template>

<script>
export default {
  data() {
    return {
      items: [
        {
          title: 'Logout'
        }
      ]
    }
  },
  methods: {
    menuItemClicked(itemIndex) {
      switch (itemIndex) {
        case 0:
          this.$store.dispatch('Authentication/logout')
      }
    }
  },
  computed: {
    title() {
      return this.$store.state.title
    }
  }
}
</script>

<style scoped>
</style>
