package com.ada.sigorta.policy.repository;



import com.ada.sigorta.policy.model.Policy;
import com.ada.sigorta.customer.model.Customer;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface PolicyRepository extends JpaRepository<Policy, Long> {
    List<Policy> findByCustomer(Customer customer);
}
