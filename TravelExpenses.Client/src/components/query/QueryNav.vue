<template>
  <div>
    <v-select
      :items="currencies"
      v-model="currency"
      return-object
      box
      background-color="white"
      color="primary"
      label="Select Display Currency"
    >
      <template slot="selection" slot-scope="data">
        <div>
          <span>
            <strong>{{ data.item.isoCode }}</strong>
            - {{ data.item.currencyName }}
          </span>
        </div>
      </template>
      <template slot="item" slot-scope="data">
        <div>
          <span>
            <strong>{{ data.item.isoCode }}</strong>
            - {{ data.item.currencyName }}
          </span>
        </div>
      </template>
    </v-select>
    <icon-text-card
      color="blue"
      icon="assessment"
      title="Locations total comparison"
      class="mb-2"
      :active="!!currency"
      @click="navLocTotalComp"
    >
      Compares the total amount spent in each location
    </icon-text-card>
    <icon-text-card
      color="purple"
      icon="assessment"
      title="Locations categories comparison"
      :active="!!currency"
      @click="navLocCatComp"
    >
      Compares categories in each location
    </icon-text-card>
  </div>
</template>

<script>
import IconTextCard from '@/components/common/IconTextCard.vue'
import QueryWindows from '@/common/enums/QueryWindows.js'
import { mapState } from 'vuex'

export default {
  components: {
    IconTextCard
  },
  methods: {
    navLocTotalComp() {
      this.$store.dispatch('Query/setQueryWindow', QueryWindows.locTotalComp)
    },
    navLocCatComp() {
      this.$store.dispatch('Query/setQueryWindow', QueryWindows.locCatComp)
    }
  },
  computed: {
    ...mapState('Currency', ['currencies']),
    currency: {
      get() {
        const id = this.$store.getters['User/defaultQueryCurrencyId']
        const ccy = this.$store.getters['Currency/findCurrency'](id)
        return ccy
      },
      set(val) {
        this.$store.dispatch('User/setDefaultQueryCurrencyId', val.id)
      }
    }
  }
}
</script>

<style scoped></style>
