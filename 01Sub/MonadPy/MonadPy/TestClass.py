
class Person:
    def __init__(self):
        self.name = None 


class Account:

    def __init__(self):
        self.id  = None 

class Balance:

    def __init__(self):
        self.cash  = None 
        


alice = Person()
alice.name = "Alice"

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

def get_loan2(name):
    account = get_account(name)
    if not account:
        return None
    balance = get_balnce(account)
    if not balance:
        return None
    loan = get_qualified_amount(balance)
    return loan 

def bind(v,f):
    if(v):
        return f(v)
    else:
        return None;

def nbind(v,f): return f(v) if v else None


mbind((alice,None),get_account)

def m_pipe(val,fns):
    m_val = val
    for f in fns:
        m_val = bind(m_val,f)
    return m_val

fns = [get_account,get_balnce,get_qualified_amount]
m_pipe(alice,fns)



def get_loanbind(name):
    m_account = get_account(name)
    m_balance = bind(m_account,get_balnce)
    m_loan = bind(m_balance,get_qualified_amount)
    return m_loan







##get_qualified_amount( get_balnce (get_account(alice)))
##
##alice | get_account | get_balance | get_qualified_amount
##
##( -> get_account get_balance get_qualified_amount)(alice)
##
##alice.get_account().get_balance().get_qualified_amount()





#####################
def unit(val): return(val,None)
def mnbind(mval,mf): return mf(mval[0]) if mval[0] else mval 

def mnbind1(mval,mf):
    value = mval[0]
    error = mval[1]
    if not error:
        return mf(value)
    else:
        return mval

def mnpipe(val, funs):
    m_val = unit(val)
    for f in funs:
        m_val= mnbind(m_val,f)
    return m_val

def m_chain(*funs):
    return funs


mnpipe(funs)







