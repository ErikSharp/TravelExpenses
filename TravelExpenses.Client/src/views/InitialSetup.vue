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
          <location1/>
        </v-window-item>
        <v-window-item>
          <location2/>
        </v-window-item>
        <v-window-item>
          <location3/>
        </v-window-item>
        <v-window-item>
          <location4/>
        </v-window-item>
        <v-window-item>
          <currency1/>
        </v-window-item>
        <v-window-item>
          <currency2/>
        </v-window-item>
        <v-window-item>
          <currency3/>
        </v-window-item>
        <v-window-item>
          <categories1/>
        </v-window-item>
        <v-window-item>
          <categories2/>
        </v-window-item>
        <v-window-item>
          <categories3/>
        </v-window-item>
        <v-window-item>
          <categories4/>
        </v-window-item>
        <v-window-item>
          <keywords1/>
        </v-window-item>
        <v-window-item>
          <keywords2/>
        </v-window-item>
        <v-window-item>
          <keywords3/>
        </v-window-item>
        <v-window-item>
          <keywords4/>
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
import Categories3 from '@/components/initialSetup/Categories3.vue'
import Categories4 from '@/components/initialSetup/Categories4.vue'
import Currency1 from '@/components/initialSetup/Currency1.vue'
import Currency2 from '@/components/initialSetup/Currency2.vue'
import Currency3 from '@/components/initialSetup/Currency3.vue'
import Finish from '@/components/initialSetup/Finish.vue'
import Keywords1 from '@/components/initialSetup/Keywords1.vue'
import Keywords2 from '@/components/initialSetup/Keywords2.vue'
import Keywords3 from '@/components/initialSetup/Keywords3.vue'
import Keywords4 from '@/components/initialSetup/Keywords4.vue'
import Location1 from '@/components/initialSetup/Location1.vue'
import Location2 from '@/components/initialSetup/Location2.vue'
import Location3 from '@/components/initialSetup/Location3.vue'
import Location4 from '@/components/initialSetup/Location4.vue'

export default {
  components: {
    Introduction,
    Categories1,
    Categories2,
    Categories3,
    Categories4,
    Currency1,
    Currency2,
    Currency3,
    Finish,
    Keywords1,
    Keywords2,
    Keywords3,
    Keywords4,
    Location1,
    Location2,
    Location3,
    Location4
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
