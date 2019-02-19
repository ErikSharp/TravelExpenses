<template>
  <v-dialog v-model="dialog" max-width="290">
    <v-btn slot="activator" flat class="primary my-3">Enter Amount</v-btn>
    <v-card>
      <v-card-title>
        User the calculator below to enter the amount{{
          currencyString ? ` of ${currencyString} on-hand` : ''
        }}
      </v-card-title>
      <v-container grid-list-md class="pt-0">
        <v-text-field
          class="readout font-weight-bold"
          readonly
          solo
          value="Erik"
        ></v-text-field>
        <v-layout row wrap>
          <v-flex xs4 v-for="n in 12" :key="n">
            <v-btn large round class="input px-0 font-weight-bold display-1">{{
              getKeyString(n)
            }}</v-btn>
          </v-flex>
        </v-layout>
      </v-container>
      <v-card-actions>
        <v-btn color="primary" @click="dialog = false">CLOSE</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    currencyString: String
  },
  data() {
    return {
      dialog: false
    }
  },
  methods: {
    getKeyString(index) {
      switch (index) {
        case 1:
          return '7'
        case 2:
          return '8'
        case 3:
          return '9'
        case 4:
          return '4'
        case 5:
          return '5'
        case 6:
          return '6'
        case 7:
          return '1'
        case 8:
          return '2'
        case 9:
          return '3'
        case 10:
          return '+'
        case 11:
          return '0'
        case 12:
          return '.'
      }
    }
  }
}
</script>

<style scoped>
.v-btn,
.input {
  min-width: 60px;
}
</style>
