<template>
  <v-card
    light
    class="my-1"
    @click="selectTransaction()"
    :class="{
      selected: transactionSelected,
      'py-1': !transaction.keywordIds.length
    }"
  >
    <v-flex>
      <v-layout align-center justify-start row fill-height>
        <v-flex shrink>
          <v-avatar size="70" class="mx-2 elevation-5" :class="iconColor">
            <v-icon size="45" class="white--text">{{ icon }}</v-icon>
          </v-avatar>
        </v-flex>
        <v-flex>
          <v-card-text class="white py-1 px-0 border-right">
            <p>
              <strong>Title:</strong>
              {{ transaction.title }}
            </p>
            <p>
              <strong>Category:</strong>
              {{ getCategoryString(transaction.categoryId) }}
            </p>
            <p>
              <strong>Amount:</strong>
              {{
                `${amountString} ${getCurrencyIsoString(
                  transaction.currencyId
                )}`
              }}
            </p>
            <p v-if="transaction.keywordIds.length" style="display: inline">
              <strong>Keywords:</strong>
            </p>
            <v-chip small v-for="(id, i) in transaction.keywordIds" :key="i">
              {{ getKeywordName(id) }}
            </v-chip>
          </v-card-text>
        </v-flex>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
import { LossGain } from '@/common/constants/StringConstants.js'
import { toLocaleStringWithEndingZero } from '@/common/StringUtilities.js'
import { mapState } from 'vuex'

export default {
  props: {
    transaction: Object
  },
  created() {
    this.setIcon(this.transaction)
  },
  data() {
    return {
      iconColor: '',
      icon: ''
    }
  },
  methods: {
    selectTransaction() {
      this.$store.dispatch(
        'Transaction/setSelectedTransaction',
        this.transaction
      )
    },
    getCategoryString(id) {
      let category = this.$store.state.Category.categories.find(
        c => c.id === id
      )
      if (category) {
        return category.categoryName
      } else if (this.$store.state.Category.lossGainCategory.id === id) {
        return this.$store.state.Category.lossGainCategory.categoryName
      } else {
        return 'unknown'
      }
    },
    getKeywordName(id) {
      let keyword = this.$store.getters['Keyword/findKeyword'](id)
      if (keyword) {
        return keyword.keywordName
      }

      return 'unknown'
    },
    getCurrencyIsoString(id) {
      let currency = this.$store.getters['Currency/findCurrency'](id)
      if (currency) {
        return `${currency.isoCode} - ${currency.currencyName}`
      }

      return ''
    },
    setIcon(transaction) {
      let cat = null

      if (
        this.lossGainCategory &&
        transaction.categoryId === this.lossGainCategory.id
      ) {
        cat = this.lossGainCategory
      } else {
        cat = this.$store.state.Category.categories.find(
          c => c.id === transaction.categoryId
        )
      }

      switch (cat.categoryName) {
        case LossGain:
          if (transaction.amount > 0) {
            this.iconColor = 'red'
            this.icon = 'trending_down'
          } else {
            this.iconColor = 'green'
            this.icon = 'trending_up'
          }
          break
        case 'Transportation':
          this.iconColor = 'indigo'
          this.icon = 'commute'
          break
        case 'Dining':
          this.iconColor = 'orange'
          this.icon = 'fastfood'
          break
        case 'Groceries':
          this.iconColor = 'green darken-2'
          this.icon = 'local_grocery_store'
          break
        case 'Entertainment':
          this.iconColor = 'pink lighten-2'
          this.icon = 'theaters'
          break
        case 'Accommodations':
          this.iconColor = 'purple lighten-2'
          this.icon = 'local_hotel'
          break
        case 'Utilities':
          this.iconColor = 'green lighten-1'
          this.icon = 'power'
          break
        case 'Medical':
          this.iconColor = 'red darken-1'
          this.icon = 'local_hospital'
          break
        case 'Fees':
          this.iconColor = 'blue lighten-2'
          this.icon = 'local_atm'
          break
        case 'Deposit':
          this.iconColor = 'cyan lighten-2'
          this.icon = 'attach_money'
          break
        case 'Non-trip':
          this.iconColor = 'orange'
          this.icon = 'card_giftcard'
          break
        default:
          this.iconColor = 'blue'
          this.icon = 'live_help'
          break
      }
    }
  },
  computed: {
    ...mapState('Category', ['lossGainCategory']),
    transactionSelected() {
      return (
        this.$store.state.Transaction.selectedTransaction == this.transaction
      )
    },
    amountString() {
      return toLocaleStringWithEndingZero(Math.abs(this.transaction.amount))
    }
  }
}
</script>

<style scoped>
.v-card p {
  margin: 0;
  height: 22px;
  overflow: hidden;
}

.v-chip {
  margin: 0 3px 3px 5px !important;
}

.v-expansion-panel .v-card {
  border-radius: 10px;
}

.border-right {
  border-radius: 10px;
}

.selected {
  border: solid #7b1fa2 5px;
}

/* chip text was going over add edit delete buttons */
.v-chip {
  z-index: 0;
}
</style>
