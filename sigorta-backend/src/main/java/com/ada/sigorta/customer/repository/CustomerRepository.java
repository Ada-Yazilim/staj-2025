package com.ada.sigorta.customer.repository;



import com.ada.sigorta.customer.model.Customer;
import com.ada.sigorta.user.model.Users;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface CustomerRepository extends JpaRepository<Customer, Long> {
    Customer findByUser(Users user);
    Optional<Customer> findById(Long id);
}

