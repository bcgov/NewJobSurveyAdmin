import { deserializeArray, plainToClass } from 'class-transformer'
import React, { useEffect, useState } from 'react'
import { requestJSONWithErrorHandler } from '../../../../helpers/requestHelpers'
import { FixTypeLater } from '../../../../types/FixTypeLater'
import CollectionSelect, {
  CollectionSelectProps,
  NameValuePair
} from './CollectionSelect'

interface Props extends CollectionSelectProps<NameValuePair> {
  itemLoadUrl: string
}

const DynamicCollectionSelect = (props: Props): JSX.Element => {
  const [valuesFromApi, setValuesFromApi] = useState<string[]>([])
  const [items, setItems] = useState<NameValuePair[]>([])

  const { itemLoadUrl } = props

  // On mount, load the values
  useEffect(() => {
    const populateData = async (): Promise<void> => {
      await requestJSONWithErrorHandler(
        `api/${itemLoadUrl}`,
        'get',
        null,
        'EMPLOYEE_NOT_FOUND',
        (responseJSON: string): void => {
          console.log('responseJSON', responseJSON)
          setValuesFromApi((responseJSON as unknown) as string[])
        }
      )
    }

    populateData()

    console.log('Dynamic load', itemLoadUrl)
  }, [])

  // When the values change, map them
  useEffect(() => {
    setItems(valuesFromApi.map(v => ({ name: v, value: v })))
  }, [valuesFromApi])

  return (
    <div className="DynamicCollectionSelect">
      <CollectionSelect<NameValuePair> items={items} {...props} />
    </div>
  )
}

export default DynamicCollectionSelect
