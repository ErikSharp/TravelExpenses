<template>
  <div>
    <v-layout row color="pink">
      <v-flex xs12>
        <span class="headline">Menu</span>

        <v-spacer></v-spacer>

        <v-menu bottom left>
          <v-btn slot="activator" icon>
            <v-icon color="black">more_vert</v-icon>
          </v-btn>

          <v-list>
            <v-list-tile v-for="(item, i) in items" :key="i" @click="menuItemClicked(i)">
              <v-list-tile-title>{{ item.title }}</v-list-tile-title>
            </v-list-tile>
          </v-list>
        </v-menu>
      </v-flex>
    </v-layout>
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
          alert('logout')
      }
    }
  }
}
</script>

<style scoped>
</style>
