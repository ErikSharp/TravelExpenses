<template>
  <div>
    <v-toolbar dark color="primary">
      <v-toolbar-title class="white--text">{{$store.state.InitialSetup.title}}</v-toolbar-title>
    </v-toolbar>
    <v-container>
      <v-window v-model="$store.state.InitialSetup.window">
        <v-window-item>
          <introduction/>
        </v-window-item>
        <v-window-item>
          <location/>
        </v-window-item>
        <v-window-item>
          <categories1/>
        </v-window-item>
        <v-window-item>
          <categories2/>
        </v-window-item>
        <v-window-item>
          <keywords1/>
        </v-window-item>
        <v-window-item>
          <keywords2/>
        </v-window-item>
        <v-window-item>
          <finish/>
        </v-window-item>
      </v-window>
    </v-container>
  </div>
</template>

<script>
import Introduction from '@/components/initialSetup/Introduction.vue'
import Categories1 from '@/components/initialSetup/Categories1.vue'
import Categories2 from '@/components/initialSetup/Categories2.vue'
import Finish from '@/components/initialSetup/Finish.vue'
import Keywords1 from '@/components/initialSetup/Keywords1.vue'
import Keywords2 from '@/components/initialSetup/Keywords2.vue'
import Location from '@/components/initialSetup/Location.vue'

export default {
  components: {
    Introduction,
    Categories1,
    Categories2,
    Finish,
    Keywords1,
    Keywords2,
    Location
  },
  data() {
    return {
      title: 'Initial Setup'
    }
  }
}
</script>

<style scoped>
</style>
