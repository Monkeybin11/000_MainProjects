def m_chain(*funs):
    return funs

def get_account(person):
    if person.name == "Alice" : return (1,None)
    elif person.name == "Bob" : return (2,None)
    else : return (None , "No acct for '%s'" %person.name)  

def get_balnce(account):
    if account.id == 1: return (1000000 , None)
    elif account.id == 2: return (75000 , None)
    else : return (None , "No balance ,  acct for '%s'" %account.id) 

def get_qualified_amount(balance):
    if balance.cash > 200000 : return (balance,None)
    else : return (None , "Insufficient funds: %s" % balance.cash) 

def get_loan(name):
    account = get_account(name)
    balance = get_balnce(account)
    loan = get_qualified_amount(balance)
    return loan     


temp = m_chain(get_account,get_balnce)


