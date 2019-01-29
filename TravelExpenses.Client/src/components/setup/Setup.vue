<template>
  <v-container>
    <v-window v-model="setupWindow">
      <v-window-item>
        <setup-nav />
      </v-window-item>
      <v-window-item>
        <locations />
      </v-window-item>
      <v-window-item>
        <categories />
      </v-window-item>
      <v-window-item>
        <keywords />
      </v-window-item>
    </v-window>
  </v-container>
</template>

<script>
import SetupNav from '@/components/setup/SetupNav.vue'
import Locations from '@/components/setup/location/Locations.vue'
import Categories from '@/components/setup/category/Categories.vue'
import Keywords from '@/components/setup/keyword/Keywords.vue'

export default {
  components: {
    SetupNav,
    Locations,
    Categories,
    Keywords
  },
  methods: {
    navigate(window) {
      this.setupWindow = window
    }
  },
  computed: {
    setupWindow() {
      return this.$store.state.SetupData.setupWindow
    }
  }
}
</script>

<style scoped></style>
