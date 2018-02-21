from sklearn import preprocessing
import pandas as pd
import seaborn as sns
import matplotlib.pyplot as plt
import numpy as np


l1 = np.asarray([1,2,2,5])
l2 = np.asarray([2,2,2,14])
index = ['ll1','ll2']

pdata_sample = pd.DataFrame(np.column_stack([l1,l2]) , columns = index)
print(pdata_sample.head())

pdata = pd.DataFrame({'F1':l1,'F2':l2})


corr = pdata.corr()
sns.heatmap(corr, 
        xticklabels=corr.columns,
        yticklabels=corr.columns,
        cmap='viridis')

plt.show()

print(corr)

print(pdata.describe())
print(pdata)

minmaxsclr = preprocessing.MinMaxScaler()

np_scaled = minmaxsclr.fit_transform(pdata)

temp = pd.DataFrame(np_scaled)
print(temp)

np_scaled0 = minmaxsclr.fit_transform(pdata.ix[:,0])
np_scaled1 = minmaxsclr.fit_transform(pdata.ix[:,1])

normalized = pd.DataFrame({'F1':np_scaled0,'F2':np_scaled1 , 'Total1':np_scaled[:,0] , 'Total2':np_scaled[:,1]})

print(normalized)



