<template>
  <v-container class="ma-0 pa-0">
    <h2 class="white--text mb-2">Select category color and icon</h2>
    <v-card class="pa-2">
      <h3>Color</h3>
      <v-layout row wrap align-center class="mt-2">
        <v-flex xs2>
          <p class="ma-0 pa-0">Red</p>
        </v-flex>
        <v-flex xs10>
          <v-slider v-model="red" max="255" hide-details color="#F00" class="ma-0 pa-0 mr-3"></v-slider>
        </v-flex>
        <v-flex xs2>
          <p class="ma-0 pa-0">Green</p>
        </v-flex>
        <v-flex xs10>
          <v-slider v-model="green" max="255" hide-details color="#0F0" class="ma-0 pa-0 mr-3"></v-slider>
        </v-flex>
        <v-flex xs2>
          <p class="ma-0 pa-0">Blue</p>
        </v-flex>
        <v-flex xs10>
          <v-slider v-model="blue" max="255" hide-details color="#00F" class="ma-0 pa-0 mr-3"></v-slider>
        </v-flex>
      </v-layout>
      <v-divider class="my-2"/>
      <h3 class="mb-2">Icon</h3>
      <v-container grid-list-md class="ma-0 pa-0">
        <v-layout row wrap>
          <v-flex v-for="i in 24" :key="i" xs2>
            <icon-selector
              :iconData="{
                id: i,
                iconColor: color,
                iconString: iconSamples[i - 1]
              }"
              :selectedId="selectedIcon.id"
              @selected="onIconSelection"
            />
          </v-flex>
        </v-layout>
      </v-container>
      <!-- <v-text-field
        v-model="selectedIcon.iconString"
        label="Enter category name"
        box
        background-color="white"
        color="primary"
      ></v-text-field>-->
    </v-card>
    <v-layout row justify-center class="mt-2">
      <v-btn :disabled="selectedIcon.id === 0" dark color="primary">CONFIRM</v-btn>
    </v-layout>
  </v-container>
</template>

<script>
import IconSelector from '@/components/setup/category/IconSelector.vue'

const padLeadingZero = hex => {
  if (hex.length < 2) {
    hex = '0' + hex
  }

  return hex
}

export default {
  components: {
    IconSelector
  },
  data() {
    return {
      red: 219,
      green: 57,
      blue: 168,
      selectedIcon: {
        id: 0,
        iconString: ''
      },
      iconSamples: [
        'flight_takeoff',
        'favorite',
        'commute',
        'motorcycle',
        'pets',
        'rowing',
        'shopping_cart',
        'music_video',
        'movie',
        'contact_phone',
        'waves',
        'headset',
        'phone_android',
        'audiotrack',
        'fastfood',
        'directions_run',
        'local_dining',
        'local_gas_station',
        'local_pizza',
        'local_grocery_store',
        'local_hotel',
        'local_taxi',
        'subway',
        'fitness_center'
      ]
    }
  },
  methods: {
    onIconSelection(iconData) {
      this.selectedIcon = iconData
    }
  },
  computed: {
    color() {
      let red = padLeadingZero(this.red.toString(16))
      let green = padLeadingZero(this.green.toString(16))
      let blue = padLeadingZero(this.blue.toString(16))

      const result = `#${red}${green}${blue}`
      return result
    }
  }
}
</script>

<style scoped>
</style>
