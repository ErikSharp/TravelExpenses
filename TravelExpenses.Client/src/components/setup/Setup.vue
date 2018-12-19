<template>
  <div>
    <v-window v-model="setupWindow">
      <v-window-item>
        <setup-nav/>
      </v-window-item>
      <v-window-item>
        <countries/>
      </v-window-item>
      <v-window-item>
        <locations/>
      </v-window-item>
      <v-window-item>
        <currencies/>
      </v-window-item>
      <v-window-item>
        <categories/>
      </v-window-item>
      <v-window-item>
        <keywords/>
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import SetupNav from '@/components/setup/SetupNav.vue'
import Countries from '@/components/setup/Countries.vue'
import Locations from '@/components/setup/Locations.vue'
import Currencies from '@/components/setup/Currencies.vue'
import Categories from '@/components/setup/Categories.vue'
import Keywords from '@/components/setup/Keywords.vue'

export default {
  components: {
    SetupNav,
    Countries,
    Locations,
    Currencies,
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

<style scoped>
</style>
